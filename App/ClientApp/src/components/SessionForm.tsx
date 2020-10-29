import React, { useContext, useEffect, useState } from "react";
import { MockSessionApi } from "../api/MockSessionApi";
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

function GenreCheckbox({ genre }: { genre: string }) {
  let state = useContext(SessionFormContext);
  let isChecked = state.selectedGenres.get.filter(e => e === genre).length > 0;
  let [checked, setChecked] = useState(isChecked)
  let toggle = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!checked) {
      let selectedGenres = state.selectedGenres.get;
      selectedGenres.push(genre);
      state.selectedGenres.set(selectedGenres);
    } else {
      let selectedGenres = state.selectedGenres.get.filter(e => e !== genre);
      state.selectedGenres.set(selectedGenres);
    }
    setChecked(!checked);
  }

  return (
    <li key={genre}>
      <input
        id={genre}
        type="checkbox"
        onChange={e => toggle(e)}
        checked={checked}
      />
      <label htmlFor={genre}>{genre}</label>
    </li>
  )
}

function FormContent() {
  let state = useContext(SessionFormContext);
  let submit = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    event.preventDefault();
  }

  return (
    state.loading?.get ?
      <Spinner /> :
      <div className="formContent">
        <div>
          <input type="text" placeholder="SessionName"
            value={state.name.get} onChange={e => state.name.set(e.target.value)} />
        </div>
        {state.genres?.get.map(genre => <GenreCheckbox key={genre} {...{ genre }} />)}
        <button onClick={e => submit(e)}>Submit</button>
      </div >
  );

}

type state<T> = { get: T, set: (value: T) => void }
type SessionFormContextProps = {
  loading: state<boolean>,
  genres: state<string[]>,
  selectedGenres: state<string[]>
  name: state<string>,
}

const SessionFormContext = React.createContext<SessionFormContextProps>({
  loading: {
    get: true, set: () => { }
  },
  genres: {
    get: [], set: () => { }
  },
  selectedGenres: {
    get: [], set: () => { }
  },
  name: {
    get: '', set: () => { }
  }
});
function SessionFormState(props: { children: React.ReactNode }) {
  let [genres, setGenres] = useState<string[]>([]);
  let [selectedGenres, setSelectedGenres] = useState<string[]>([]);
  let [loading, setLoading] = useState(true);
  let [name, setName] = useState('');

  useEffect(() => {
    setGenres(MockSessionApi.GetGenres());
    setLoading(false);
  }, []);

  return (
    <SessionFormContext.Provider value={{
      loading: { get: loading, set: setLoading },
      genres: { get: genres, set: setGenres },
      selectedGenres: { get: selectedGenres, set: setSelectedGenres },
      name: { get: name, set: setName }
    }}>
      {
        props.children
      }
    </SessionFormContext.Provider>
  )
}