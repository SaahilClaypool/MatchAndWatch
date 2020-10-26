import React, { Component } from "react";
import { Route, Switch } from "react-router";
import { Layout } from "./components/Layout";
import { Counter } from "./components/Counter";
import AuthorizeRoute from "./components/api-authorization/AuthorizeRoute";
import ApiAuthorizationRoutes from "./components/api-authorization/ApiAuthorizationRoutes";
import { ApplicationPaths } from "./components/api-authorization/ApiAuthorizationConstants";
import { NoRoute } from "./components/Error";

import "./custom.css";
import { HomePage } from "./pages/HomePage";
import { Routes } from "./Routes";
import { SessionRoutes } from "./pages/SessionRoutes";
import authService from "./components/api-authorization/AuthorizeService";
import { FetchData } from "./components/FetchData";


export default class App extends Component {
  static displayName = App.name;

  render() {
    const token = authService.getAccessToken().then(token => console.log(token));
    return (
      <Layout>
        <Switch>
          <Route exact path={Routes.Root} component={HomePage} />
          <Route path="/counter" component={Counter} />
          <AuthorizeRoute
            path={Routes.Session.Base}
            component={SessionRoutes}
          />
          <AuthorizeRoute
            path="/Weather"
            component={FetchData}
          />
          <Route
            path={ApplicationPaths.ApiAuthorizationPrefix}
            component={ApiAuthorizationRoutes}
          />
          <Route
            children={(match) => <NoRoute path={match.location.pathname} />}
          />
        </Switch>
      </Layout>
    );
  }
}
