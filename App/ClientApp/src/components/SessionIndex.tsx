import React, { useState, useEffect } from "react";
import { Spinner } from "../components/Spinners";
import PropTypes from "prop-types";
import { Session } from "../services/Session";
import { MockSessionApi } from "../api/MockSessionApi";
import { Link } from "react-router-dom";
import "./SessionIndex.css";
import { Routes } from "../Routes";

export function SessionIndexLoader(props: { user: string }) {
  let [isLoading, setLoading] = useState(false);
  let [sessions, setSessions] = useState<Session[]>([]);

  useEffect(() => {
    let sessions = MockSessionApi.GetSessions();
    setSessions(sessions);
    setLoading(false);
  }, [props.user]);

  return (
    <div className="SessionIndex">
      {isLoading ? (
        <Spinner />
      ) : (
        <>
          <SessionIndex existingSessions={sessions} />
        </>
      )}
    </div>
  );
}

export interface SessionIndexProps {
  existingSessions: Session[];
}

export function SessionIndex(props: SessionIndexProps) {
  return (
    <React.Fragment>
      <h2>Previous Sessions</h2>
      <hr />
      <blockquote>TODO: turn into links</blockquote>
      <ul>
        {props.existingSessions.map((session) => (
          <SessionRow {...session} />
        ))}
      </ul>
    </React.Fragment>
  );
}

export function SessionRow(props: Session) {
  return (
    <div className="SessionRow">
      <div className="Name">{props.name}</div>
      <div className="Meta">{props.creator}</div>
    </div>
  );
}
