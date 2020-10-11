import React from "react";
import { SessionForm } from "../components/SessionForm";

export function NewSession() {
  return (
    <div className="NewSession">
      <Title />
      <SessionForm />
    </div>
  );
}

const Title = () => <h2>Create a new session</h2>;
