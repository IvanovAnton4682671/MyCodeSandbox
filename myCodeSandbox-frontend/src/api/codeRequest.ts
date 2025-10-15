import axios from "axios";

const host = "http://localhost:5164/api/code/execution"

export const postCodeExecution = async (code: string) => {
    try {
        const response = await axios.post(
            host,
            {
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