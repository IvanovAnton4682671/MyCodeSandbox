import type { MyThemeType } from "../themes";
import { Flex, Text, Button } from "@radix-ui/themes";
import { SunIcon, MoonIcon } from "@radix-ui/react-icons";

interface MyHeaderProps {
    onThemeChange: (currentTheme: MyThemeType) => void;
}

function MyHeader({ onThemeChange }: MyHeaderProps) {
    return(
        <Flex width="100%" height="100%" direction="row" justify="between" align="center" style={{ paddingLeft: "30px", paddingRight: "30px" }}>
            <Text size="7" weight="bold">MyCodeSandbox</Text>
            <Flex direction="row" justify="center" align="center" gap="3">
                <Button variant="solid" onClick={ () => onThemeChange("light") }>
                    <SunIcon fontSize="20px"/>
                    Светлая
                </Button>
                <Button variant="solid" onClick={ () => onThemeChange("dark") }>
                    <MoonIcon fontSize="20px"/>
                    Тёмная
                </Button>
            </Flex>
        </Flex>
    )
}

export default MyHeader;