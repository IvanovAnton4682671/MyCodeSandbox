import { Flex, Select, Box, ScrollArea, TextArea, Button } from "@radix-ui/themes";
import { PlayIcon, TrashIcon } from "@radix-ui/react-icons";

function MyHomeContent() {
    return(
        <Flex width="100%" height="100%" direction="column" justify="center" align="center" gap="3">
            <Select.Root defaultValue="c#">
                <Select.Trigger />
                <Select.Content>
                    <Select.Item value="c#">C#</Select.Item>
                    <Select.Item value="python">Python</Select.Item>
                    <Select.Item value="java">Java</Select.Item>
                </Select.Content>
            </Select.Root>
            <Flex direction="row" width="80%" height="70%" justify="center" align="center" gap="3">
                <Box style={{ width: "100%", height: "100%" }}>
                    <ScrollArea type="auto" scrollbars="vertical" style={{ width: "100%", height: "100%" }}>
                        <TextArea placeholder="Пишите ваш код здесь..." resize="none" style={{ width: "100%", height: "100%", wordWrap: "normal" }}/>
                    </ScrollArea>
                </Box>
                <Box style={{ width: "100%", height: "100%" }}>
                    <ScrollArea type="auto" scrollbars="both" style={{ width: "100%", height: "100%" }}>
                        <TextArea placeholder="Результат работы вашего кода будет здесь..." resize="none" readOnly style={{ width: "100%", height: "100%", wordWrap: "normal" }}/>
                    </ScrollArea>
                </Box>
            </Flex>
            <Flex direction="row" justify="center" align="center" gap="3">
                <Button>
                    <PlayIcon fontSize="20px"/>
                    Запустить
                </Button>
                <Button>
                    <TrashIcon fontSize="20px"/>
                    Очистить
                </Button>
            </Flex>
        </Flex>
    )
}

export default MyHomeContent;