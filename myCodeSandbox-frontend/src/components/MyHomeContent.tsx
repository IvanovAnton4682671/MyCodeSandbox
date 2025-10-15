import type { MyThemeType } from "../themes";
import React from "react";
import type { MyCodeLanguages } from "./languages";
import { postCodeExecution } from "../api/codeRequest";
import { Flex, Select, Box, Button } from "@radix-ui/themes";
import { Editor } from "@monaco-editor/react";
import { PlayIcon, TrashIcon } from "@radix-ui/react-icons";

interface MyHomeContentProps {
    currentTheme: MyThemeType;
}

function MyHomeContent({ currentTheme }: MyHomeContentProps) {
    const [language, setLanguage] = React.useState<MyCodeLanguages>("python");
    const [code, setCode] = React.useState<string>("");
    const [output, setOutput] = React.useState<string>("");

    const getMonacoLanguage = (lang: MyCodeLanguages) => {
        switch (lang) {
            case "python": return "python";
            case "java": return "java";
            case "cpp": return "cpp";
            default: return "python";
        }
    };

    const handleRunCode = async () => {
        const response = await postCodeExecution(language, code);
        setOutput(response);
    };

    const handleClear = () => {
        setCode("");
        setOutput("");
    };

    return(
        <Flex width="100%" height="100%" direction="column" justify="center" align="center" gap="3">
            <Select.Root value={ language } onValueChange={ (value: MyCodeLanguages) => setLanguage(value) }>
                <Select.Trigger />
                <Select.Content>
                    <Select.Item value="python">Python</Select.Item>
                    <Select.Item value="java">Java</Select.Item>
                    <Select.Item value="cpp">C++</Select.Item>
                </Select.Content>
            </Select.Root>
            <Flex direction="row" width="80%" height="70%" justify="center" align="center" gap="3">
                <Box style={{ width: "100%", height: "100%" }}>
                    <Editor 
                        theme={ currentTheme === "light" ? "vs" : "vs-dark" }
                        language={ getMonacoLanguage(language) }
                        value={ code }
                        onChange={ (value) => setCode(value || "") }
                        options={{
                            minimap: { enabled: false },
                            scrollBeyondLastLine: false,
                            automaticLayout: true,
                            fontSize: 14,
                            wordWrap: "off",
                            lineNumbers: "on",
                            folding: true,
                            lineNumbersMinChars: 3,
                            scrollbar: {
                                vertical: "visible",
                                horizontal: "visible",
                                useShadows: false
                            }
                        }}
                    />
                </Box>
                <Box style={{ width: "100%", height: "100%" }}>
                    <Editor
                        theme={ currentTheme === "light" ? "vs" : "vs-dark" }
                        language="plaintext"
                        value={ output }
                        options={{
                            minimap: { enabled: false },
                            scrollBeyondLastLine: false,
                            automaticLayout: true,
                            fontSize: 14,
                            wordWrap: "off",
                            lineNumbers: "off",
                            readOnly: true,
                            scrollbar: {
                                vertical: "visible",
                                horizontal: "visible",
                                useShadows: false
                            }
                        }}
                    />
                </Box>
            </Flex>
            <Flex direction="row" justify="center" align="center" gap="3">
                <Button variant="solid" onClick={ () => handleRunCode() }>
                    <PlayIcon fontSize="20px"/>
                    Запустить
                </Button>
                <Button variant="solid" onClick={ () => handleClear() }>
                    <TrashIcon fontSize="20px"/>
                    Очистить
                </Button>
            </Flex>
        </Flex>
    )
}

export default MyHomeContent;