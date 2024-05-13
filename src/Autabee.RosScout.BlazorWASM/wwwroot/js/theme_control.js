function SetTheme(ThemeName) {

    var Theme = document.getElementById(`Theme`);
    Theme.href = `/css/` + ThemeName;
}

function SetNavTheme(ThemeName) {

    var Theme = document.getElementById(`NavTheme`);
    Theme.href = `/css/` + ThemeName;
}