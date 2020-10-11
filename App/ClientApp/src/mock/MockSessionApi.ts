import { Session } from "../services/Session";

export class MockSessionApi {
  static GetSessions(): Session[] {
    return [
      {
        name: "RomCom w/ Sarah",
        creator: "saahil",
      },
      {
        name: "Action w/ Sarah",
        creator: "saahil",
      },
      {
        name: "Other",
        creator: "sarah",
      },
    ];
  }
}
