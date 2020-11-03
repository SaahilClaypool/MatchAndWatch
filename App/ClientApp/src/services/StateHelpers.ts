import { useState } from "react";

export type State<T> = { get: T, set: (value: T) => void }
export function useStateObject<T>(val: any): State<T> {
    var [state, setState] = useState<T>(val);
    return { get: state, set: setState }
}