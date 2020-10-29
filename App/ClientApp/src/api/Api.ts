import axios from "axios";
import authService from "../components/api-authorization/AuthorizeService";


export class Api {
    private static headers = async () => {
        const token = await authService.getAccessToken();
        return {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json'
        }

    }

    private static req = async (url: string, method: string, body?: object, extraHeaders?: object): Promise<object> => {
        let headers = await Api.headers();
        let response = await fetch(url, {
            method,
            headers: {
                ...headers,
                ...extraHeaders
            },
            body: JSON.stringify(body)
        });
        return await response.json();
    }

    public static get = async (url: string, extraHeaders?: object): Promise<object> => await Api.req(url, 'GET', extraHeaders);
    public static post = async (url: string, body: object, extraHeaders?: object): Promise<object> => await Api.req(url, 'POST', body, extraHeaders);
}