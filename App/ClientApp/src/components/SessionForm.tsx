import React, { useContext, useEffect, useState } from "react";
import { MockSessionApi } from "../api/MockSessionApi";
import { SessionApi } from "../api/SessionApi";
import { ErrorResponse, FlashError } from "./FlashError";
import { Spinner } from "./Spinners";

export function SessionForm() {
  return (
    <div>
      <FormContent />
    </div>
  )
}

function GenreCheckbox({ genre, selected }: { genre: string, selected: State<string[]> }) {
  let isChecked = selected.get.filter(e => e === genre).length > 0;
  let [checked, setChecked] = useState(isChecked)
  let toggle = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!checked) {
      let selectedGenres = selected.get;
      selectedGenres.push(genre);
      selected.set(selectedGenres);
    } else {
      let selectedGenres = selected.get.filter(e => e !== genre);
      selected.set(selectedGenres);
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

type State<T> = { get: T, set: (value: T) => void }
function useStateObject<T>(val: any): State<T> {
  var [state, setState] = useState<T>(val);
  return { get: state, set: setState }
}

function FormContent() {
  let state = {
    loading: useStateObject<boolean>(true),
    genres: useStateObject<string[]>([]),
    selectedGenres: useStateObject<string[]>([]),
    name: useStateObject<string>(''),
    errorResponse: useStateObject<ErrorResponse | null>(null),
  };

  useEffect(() => {
    SessionApi.GetGenres().then(genres => state.genres.set(genres));
    state.loading.set(false);
  }, [])

  let submit = (event: React.MouseEvent<HTMLInputElement, MouseEvent>) => {
    event.preventDefault();
    SessionApi.CreateSession({
      name: state.name.get,
      creator: '', // set in backend
      genres: state.selectedGenres.get
    })
      .then(_ => state.errorResponse.set(null))
      .catch(e => {
        state.errorResponse.set({ errors: e })
    })

  }

  return (
    state.loading.get ?
      <Spinner /> :
      <div>
        {state.errorResponse.get !== null ? <FlashError {...state.errorResponse.get} /> : <></>}
        <form>
          <div className="formContent">
            <div>
              <input type="text" placeholder="SessionName"
                value={state.name.get} onChange={e => state.name.set(e.target.value)} />
            </div>
            {
              state.genres?.get.map(genre =>
                <GenreCheckbox selected={state.selectedGenres} key={genre} {...{ genre }} />
              )
            }
            <input type="submit" onClick={e => submit(e)} />
          </div >
        </form>
      </div >
  );

}