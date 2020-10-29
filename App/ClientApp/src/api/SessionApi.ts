import { Session } from "../services/Session";
import { Api } from "./Api";

export class SessionApi {
    static async GetSessions(): Promise<Session[]> {
        return await Api.get('Session') as Session[];
    }

    static async CreateSession(session: Session): Promise<any> {
        return await Api.post('Session', session);
    }

    static GetGenres(): string[] {
        return [
            'Comedy', 'Rom-Com', 'Action'
        ]
    }
}
