import React from "react";
import { Route, Switch } from "react-router";
import { useRouteMatch } from "react-router-dom";
import { NoRoute } from "../components/Error";
import { NewSession } from "./NewSessionPage";

export function SessionRoutes() {
  let { path } = useRouteMatch();
  return (
    <Switch>
      <Route exact path={`${path}/`} component={NewSession} />
      <Route children={(match) => <NoRoute path={match.location.pathname} />} />
    </Switch>
  );
}
