type MyTheme = {
    appearance: "light" | "dark";
    accentColor: "indigo" | "mint" | "amber";
}

const lightTheme: MyTheme = {
    appearance: "light",
    accentColor: "indigo"
};

const darkTheme: MyTheme = {
    appearance: "dark",
    accentColor: "mint"
};

const highContrastTheme: MyTheme = {
    appearance: "dark",
    accentColor: "amber"
}

export type MyThemeType = "light" | "dark" | "highContrast";

export const themes: Record<MyThemeType, MyTheme> = {
    "light": lightTheme,
    "dark": darkTheme,
    "highContrast": highContrastTheme
};