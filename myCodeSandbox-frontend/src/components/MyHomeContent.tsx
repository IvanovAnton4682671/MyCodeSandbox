import { Flex, Select, TextArea, ScrollArea, Button } from "@radix-ui/themes";
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
            <TextArea placeholder="Write your code here..." resize="none" style={{ width: "70%", height: "70%" }}>
                <ScrollArea type="auto" scrollbars="vertical" />
            </TextArea>
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