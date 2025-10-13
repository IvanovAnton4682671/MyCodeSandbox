type MyTheme = {
    appearance: "light" | "dark";
    accentColor: "plum" | "mint";
}

const lightTheme: MyTheme = {
    appearance: "light",
    accentColor: "plum"
};

const darkTheme: MyTheme = {
    appearance: "dark",
    accentColor: "mint"
};

export type MyThemeType = "light" | "dark";

export const themes: Record<MyThemeType, MyTheme> = {
    "light": lightTheme,
    "dark": darkTheme,
};