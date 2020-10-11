import React, { useContext, useEffect, useState } from "react";
import { MockSessionApi } from "../mock/MockSessionApi";
import { Spinner } from "./Spinners";

export function SessionForm() {
  return (
    <div>
      <SessionFormState>
        <FormContent />
      </SessionFormState>
    </div>
  )
}

function FormContent() {
  let state = useContext(SessionFormContext);

  return (
    state.loading?.get ? <Spinner /> : <div>Form</div>
  );

}

type state<T> = { get: T, set: (value: T) => void }
type SessionFormContextProps = {
  loading: state<boolean>
}

const SessionFormContext = React.createContext<Partial<SessionFormContextProps>>({});
function SessionFormState(props: { children: React.ReactNode }) {
  let [loading, setLoading] = useState(true);

  useEffect(() => {
    MockSessionApi.GetGenres()

    setLoading(false);
  });

  return (
    <SessionFormContext.Provider value={{
      loading: { get: loading, set: setLoading }
    }}>
      {
        props.children
      }
    </SessionFormContext.Provider>
  )
}