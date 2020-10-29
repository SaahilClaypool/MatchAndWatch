import { Session } from "../services/Session";
import { Api } from "./Api";

export class SessionApi {
    static GetSessions(): Session[] {
        Api.get('Session');
        return [];
    }

    static GetGenres(): string[] {
        return [
            'Comedy', 'Rom-Com', 'Action'
        ]
    }
}
