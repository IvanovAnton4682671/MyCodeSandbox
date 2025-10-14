import "./App.css";
import "@radix-ui/themes/styles.css";
import React from "react";
import type { MyThemeType } from "./themes";
import { themes } from "./themes";
import { Theme, Flex } from "@radix-ui/themes";
import MyHome from "./components/pages/MyHome";

function App() {
  const [currentTheme, setCurrentTheme] = React.useState<MyThemeType>("light");

  return(
    <Theme scaling="100%" radius="medium" appearance={ themes[currentTheme].appearance } accentColor={ themes[currentTheme].accentColor }>
      <Flex width="100vw" height="100vh">
        <MyHome currentTheme={ currentTheme } onThemeChange={ setCurrentTheme }/>
      </Flex>
    </Theme>
  )
}

export default App;