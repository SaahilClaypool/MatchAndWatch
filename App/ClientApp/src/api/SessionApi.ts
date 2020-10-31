import { Session } from "../services/Session";
import { Api } from "./Api";

export class SessionApi {
    static async GetSessions(): Promise<Session[]> {
        return await Api.get('api/Session') as Session[];
    }

    static async CreateSession(session: Session): Promise<any> {
        return await Api.post('api/Session', session);
    }

    static async GetGenres(): Promise<string[]> {
        return await Api.get('api/Genre').then(response => response as string[]);
    }
}
