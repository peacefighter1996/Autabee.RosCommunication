namespace Autabee.RosScout.BlazorWASM
{
    public class UserTheme
    {
        bool dark;
        public bool Dark
        {
            get => dark; set
            {
                if (dark != value)
                {
                    dark = value;
                    if (NavLinked)
                    {
                        navdark = value;
                    }
                    
                    ThemeChanged?.Invoke(this, null);
                }
            }
        }
        public string DarkString { get => Dark ? "dark" : "light"; }
        public string NavDarkString { get => navdark ? "dark" : "light"; }
        public string Nav { get; set; }
        string theme;
        public string Theme
        {
            get => theme; set
            {
                if (theme != value && value != null)
                {
                    theme = value;
                    ThemeChanged?.Invoke(this, null);
                }
            }
        }
        public string ThemeCss { get => $"{Theme}.{DarkString}.min.css"; }
        public string NavThemeCss { get => $"nav.{NavDarkString}.min.css"; }
        bool navdark;
        public bool NavDark { get => navdark; set
            {
                if (navdark != value)
                {
                    navdark = value;
                    if (NavLinked)
                    {
                        dark = value;
                    }

                    ThemeChanged?.Invoke(this, null);
                }
            }
        }

        public bool NavLinked { get; set; } = false;
        public UserTheme()
        {
        }

        public event EventHandler ThemeChanged;
    }
}
