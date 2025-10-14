import type { MyThemeType } from "../../themes";
import { Flex } from "@radix-ui/themes";
import MyHeader from "../MyHeader";
import MyHomeContent from "../MyHomeContent";

interface MyHomeProps {
    currentTheme: MyThemeType;
    onThemeChange: (currentTheme: MyThemeType) => void;
}

function MyHome({ currentTheme, onThemeChange }: MyHomeProps) {
    return(
        <Flex width="100%" height="100%" direction="column" justify="center" align="center">
            <Flex width="100%" height="10%" justify="center" align="center">
                <MyHeader onThemeChange={ onThemeChange }/>
            </Flex>
            <Flex width="100%" height="90%" justify="center" align="center">
                <MyHomeContent currentTheme={ currentTheme }/>
            </Flex>
        </Flex>
    )
}

export default MyHome;