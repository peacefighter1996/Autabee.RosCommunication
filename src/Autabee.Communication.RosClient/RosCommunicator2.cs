using Autabee.Communication.RosClient.Dto;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Moveit;
using RosSharp.RosBridgeClient.Protocols;
using System.Text.Json;
using System.Text;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Autabee.Communication.RosClient
{
    public interface ISerializer {

        byte[] Serialize(object obj);
        byte[] Serialize(object obj,Type type );
        byte[] Serialize<T>(T obj);
        DeserializedObject Deserialize(byte[] rawData);
        Message Deserialize(string JsonString, Type type);
        T Deserialize<T>(string JsonString);
    }

    public abstract class DeserializedObject
    {
        internal abstract string GetProperty(string property);
    }

    internal class MicrosoftJsonObject : DeserializedObject
    {
        private JsonElement jObject;

        internal MicrosoftJsonObject(JsonElement _jObject)
        {
            jObject = _jObject;
        }

        internal override string GetProperty(string property)
        {
            return jObject.GetProperty(property).ToString();
        }
    }

    public class MicrosoftSerializer : ISerializer
    {
        public byte[] Serialize(object obj)
            => Serialize(obj, obj.GetType());
        public byte[] Serialize<T>(T obj) 
            => Serialize(obj, typeof(T));

        public byte[] Serialize(object obj, Type type)
        {
            string json = JsonSerializer.Serialize(obj, type);
            return Encoding.ASCII.GetBytes(json);
        }

        public DeserializedObject Deserialize(byte[] buffer)
        {
            string ascii = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
            JsonElement jsonElement = JsonDocument.Parse(ascii).RootElement;
            return new MicrosoftJsonObject(jsonElement);
        }

        public Message Deserialize(string json, Type type)
        {
            return (Message)JsonSerializer.Deserialize(json, type);
        }

        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }

    public enum Oppreation
    {
        publish,
        subscribe,
        unsubscribe,
        call_service,
        service_response,
        advertise,
        unadvertise
    }

    public static class OpperationOverride
    {
        public static string GetOpString(this Oppreation op) => op switch
        {
            Oppreation.advertise => "advertise",
            Oppreation.call_service => "call_service",
            Oppreation.publish => "publish",
            Oppreation.service_response => "service_response",
            Oppreation.subscribe => "subscribe",
            Oppreation.unsubscribe => "unsubscribe",
            Oppreation.unadvertise => "unadvertise",
            _ => throw new ArgumentException("Invalid operation")
        };
    }


    public record Communication(string op, string id);
    public record Advertisement( string topic, string type, string id) : Communication(Oppreation.advertise.GetOpString(), id);
    public record Unadvertisement(string topic, string id) : Communication(Oppreation.unadvertise.GetOpString(), id);
    public record Publication(string topic, object msg, string id) : Communication(Oppreation.publish.GetOpString(), id);
    public record Publication<T> (string topic, T data, string id) : Publication(topic,data, id) where T : Message;
    public record Subscription(string topic, string type, int throttle_rate, int queue_length, int fragment_size, string compression, string id) : Communication(Oppreation.subscribe.GetOpString(), id);
    public record Unsubscription(string topic, string id) : Communication(Oppreation.unsubscribe.GetOpString(), id);
    public record ServiceCall<T>(string service, T args, int fragment_size, string compression, string id) : Communication(Oppreation.call_service.GetOpString(), id);
    public record ServiceResponse<T>(string service, T values, bool result, string id) : Communication(Oppreation.service_response.GetOpString(), id);
    public record ServiceAdvertisement(string service, string type, string id) : Communication(Oppreation.advertise.GetOpString(), id);
    public record ServiceUnadvertisement(string service, string id) : Communication(Oppreation.unadvertise.GetOpString(), id);

    public abstract class Communicator
    {
        public static string GetRosName<T>() where T : Message
        {
            return (string)typeof(T).GetField("RosMessageName").GetRawConstantValue();
        }
        public static string GetRosName(Type type)
        {
            if (!type.IsSubclassOf(typeof(Message)))
                throw new ArgumentException("Type must be a subclass of Message");
            return (string)type.GetField("RosMessageName").GetRawConstantValue();
        }
    }

    public class Publisher : Communicator
    {
        public readonly string id;
        public readonly string topic;
        public readonly Type type;
        public readonly string rosName;
        public Publisher(string id, string topic, Type type, out Advertisement advertisement)
        {
            this.id = id;
            this.topic = topic;
            this.rosName = Communicator.GetRosName(type);
            this.type = type;
            advertisement = new Advertisement(topic,rosName , id);
        }

        public Communication Publish(object message)
        {
            if (message.GetType() != type)
                throw new ArgumentException("Message type does not match the type of the publisher");
            return new Publication(topic, message, id);
        }

        public Unadvertisement Unadvertise()
        {
            return new Unadvertisement(topic, id);
        }
    }
    public class Publisher<T> : Publisher where T : Message
    {
        public Publisher(string id, string topic, out Advertisement advertisement) : base(id, topic, typeof(T), out advertisement)
        {
        }

        public Communication Publish<T>(Message message) where T : Message
        {
            if (message.GetType() != type)
                throw new ArgumentException("Message type does not match the type of the publisher");
            return new Publication<T>(topic, (T)message, id);
        }
    }


    public class AutabeeRosSocket
    {
        //topic, publisher
        private Dictionary<string, Publisher> Publishers = new Dictionary<string, Publisher>();
        public IProtocol protocol;
        public ISerializer Serializer = new MicrosoftSerializer();
        private object SubscriberLock = new object();

        public event EventHandler OnConnected;
        public event EventHandler OnClosed;

        public AutabeeRosSocket(string uri)
        {
            this.protocol = new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(uri);
            this.protocol.OnReceive += Receive;
            this.protocol.OnClosed += Closed;
            this.protocol.OnConnected += Connected;
            this.protocol.Connect();
        }


        public void Connect()
        {
            protocol.Connect();
        }
        public void Close(int millisecondsWait = 0)
        {
            bool isAnyCommunicatorActive = Publishers.Count > 0;//|| Subscribers.Count > 0 || ServiceProviders.Count > 0;

            while (Publishers.Count > 0)
                UnAdvertise(Publishers.First().Key);
            // Service consumers do not stay on. So nothing to unsubscribe/unadvertise

            if (isAnyCommunicatorActive)
            {
                Thread.Sleep(millisecondsWait);
            }

            protocol.Close();
        }

        #region Publishers

        public string Advertise<T>(string topic) where T : Message
            => CreateAdvertiser<T>(topic).id;

        public string Advertise(string topic, Type type)
        {
            if (!type.IsSubclassOf(typeof(Message)))
                throw new ArgumentException("Type must be a subclass of Message");
            return CreateAdvertiser(topic, type).id;
        }

        protected (string id, Publisher publisher) CreateAdvertiser<T>(string topic) where T : Message
            => CreateAdvertiser(topic, typeof(T));

        protected (string id, Publisher publisher) CreateAdvertiser(string topic, Type type) 
        {
            string id = topic;
            if (Publishers.ContainsKey(id))
                UnAdvertise(id);
            var publisher = new Publisher(id, topic, type, out Advertisement advertisement);
            Publishers.Add(id, publisher);
            Send(advertisement);
            return (id, publisher);
        }


        public (int,string) Publish(string topic, object message, bool CreatePublisher = false)
        {
            if (Publishers.TryGetValue(topic, out Publisher publisher))
            {
                Send(publisher.Publish(message));
                return (0, publisher.id);
            }
            else if (CreatePublisher)
            {
                var id = Advertise(topic, message.GetType());
                Send(Publishers[topic].Publish(message));
                return (1, id);
            }
            else
            {
                return (-1, "Publisher not found");
            }
        }


        public void UnAdvertise(string topic)
        {
            Send(Publishers[topic].Unadvertise());
            Publishers.Remove(topic);
        }

        #endregion

        //#region Subscribers

        //public string Subscribe<T>(string topic, SubscriptionHandler<T> subscriptionHandler, int throttle_rate = 0, int queue_length = 1, int fragment_size = int.MaxValue, string compression = "none") where T : Message
        //{
        //    string id;
        //    lock (SubscriberLock)
        //    {
        //        id = GetUnusedCounterID(Subscribers, topic);
        //        Subscription subscription;
        //        Subscribers.Add(id, new Subscriber<T>(id, topic, subscriptionHandler, out subscription, throttle_rate, queue_length, fragment_size, compression));
        //        Send(subscription);
        //    }

        //    return id;
        //}

        //public void Unsubscribe(string id)
        //{
        //    Send(Subscribers[id].Unsubscribe());
        //    Subscribers.Remove(id);
        //}
        //#endregion

        //#region ServiceProviders

        //public string AdvertiseService<Tin, Tout>(string service, ServiceCallHandler<Tin, Tout> serviceCallHandler) where Tin : Message where Tout : Message
        //{
        //    string id = service;
        //    if (ServiceProviders.ContainsKey(id))
        //        UnadvertiseService(id);

        //    ServiceAdvertisement serviceAdvertisement;
        //    ServiceProviders.Add(id, new ServiceProvider<Tin, Tout>(service, serviceCallHandler, out serviceAdvertisement));
        //    Send(serviceAdvertisement);
        //    return id;
        //}

        //public void UnadvertiseService(string id)
        //{
        //    Send(ServiceProviders[id].UnadvertiseService());
        //    ServiceProviders.Remove(id);
        //}

        //#endregion

        //#region ServiceConsumers

        //public string CallService<Tin, Tout>(string service, ServiceResponseHandler<Tout> serviceResponseHandler, Tin serviceArguments) where Tin : Message where Tout : Message
        //{
        //    string id = GetUnusedCounterID(ServiceConsumers, service);
        //    Communication serviceCall;
        //    ServiceConsumers.Add(id, new ServiceConsumer<Tin, Tout>(id, service, serviceResponseHandler, out serviceCall, serviceArguments));
        //    Send(serviceCall);
        //    return id;
        //}

        //#endregion
        private void Send(object communication) 
        {
            protocol.Send(Serializer.Serialize(communication));
            return;
        }
        //private void Send<T>(T communication) where T : Communication
        //{
        //    protocol.Send(Serializer.Serialize<T>(communication));
        //    return;
        //}

        private void Receive(object sender, EventArgs e)
        {
            byte[] buffer = ((MessageEventArgs)e).RawData;
            DeserializedObject jsonElement = Serializer.Deserialize(buffer);

            switch (jsonElement.GetProperty("op"))
            {
                case "publish":
                    {
                        string topic = jsonElement.GetProperty("topic");
                        string msg = jsonElement.GetProperty("msg");
                        //foreach (Subscriber subscriber in SubscribersOf(topic))
                        //    subscriber.Receive(msg, Serializer);
                        return;
                    }
                case "service_response":
                    {
                        string id = jsonElement.GetProperty("id");
                        string values = jsonElement.GetProperty("values");
                        //ServiceConsumers[id].Consume(values, Serializer);
                        return;
                    }
                case "call_service":
                    {
                        string id = jsonElement.GetProperty("id");
                        string service = jsonElement.GetProperty("service");
                        string args = jsonElement.GetProperty("args");
                        //Send(ServiceProviders[service].Respond(id, args, Serializer));
                        return;
                    }
            }
        }

        //private List<Subscriber> SubscribersOf(string topic)
        //{
        //    return Subscribers.Where(pair => pair.Key.StartsWith(topic + ":")).Select(pair => pair.Value).ToList();
        //}

        //private static string GetUnusedCounterID<T>(Dictionary<string, T> dictionary, string name)
        //{
        //    int I = 0;
        //    string id;
        //    do
        //        id = name + ":" + I++;
        //    while (dictionary.ContainsKey(id));
        //    return id;
        //}
        void Closed(object? sender, EventArgs e)
        {
            OnClosed?.Invoke(this, e);
        }
        void Connected(object? sender, EventArgs e)
        {
            OnConnected?.Invoke(this, e);
        }
    }
}
