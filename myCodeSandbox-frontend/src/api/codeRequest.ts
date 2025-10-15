import axios from "axios";
import type { MyCodeLanguages } from "../components/languages";

const host = "http://localhost:5164/api/code/execution"

export const postCodeExecution = async (language: MyCodeLanguages, code: string) => {
    try {
        const response = await axios.post(
            host,
            {
                CodeLanguage: language,
                CodeInput: code
            },
            {
                headers: {
                    "Content-Type": "application/json"
                }
            }
        );
        return response.data;
    } catch (error: any) {
        console.error("Ошибка при отправке кода: ", error);
        throw error;
    }
};