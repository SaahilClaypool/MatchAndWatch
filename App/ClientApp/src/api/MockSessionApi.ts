import { Session } from "../services/Session";

export class MockSessionApi {
  static GetSessions(): Session[] {
    return [
      {
        name: "RomCom w/ Sarah",
        creator: "saahil",
        genres: []
      },
      {
        name: "Action w/ Sarah",
        creator: "saahil",
        genres: []
      },
      {
        name: "Other",
        creator: "sarah",
        genres: []
      },
    ];
  }

  static GetGenres(): string[] {
    return [
      'Comedy', 'Rom-Com', 'Action'
    ]
  }
}
