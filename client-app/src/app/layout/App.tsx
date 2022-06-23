import React, { useEffect } from "react";
import { Container } from "semantic-ui-react";
import DataDashboard from "../../features/DataDashboard/DataDashboard";
import ModalContainer from "../common/modals/ModalContainer";
import NavBar from "./NavBar";
import HomePage from "../../features/home/Homepage";
import { Route, Switch, useLocation } from "react-router-dom";
import { useStore } from "../stores/store";
import TestErrors from "../../features/errors/TestErrors";
import ServerError from "../../features/errors/ServerError";
import LoginForm from "../../features/users/LoginForm";
import NotFound from "../../features/errors/NotFound";
import AppUserDashboard from "../../features/administration/AppUserDashboard";
import LoadingComponent from "./LoadingComponents";
import { observer } from "mobx-react-lite";
import AppUserForm from "../../features/administration/form/AppUserForm";
import PrivateRoute from "./PrivateRoute";
import { ToastContainer } from "react-toastify";

function App() {
  const location = useLocation();
  const { commonStore} = useStore();

  useEffect(() => {
    if (commonStore.token) {
      commonStore.getUser().finally(() => commonStore.setAppLoaded());
    } else {
      commonStore.setAppLoaded();
    }
  }, [commonStore]);

  if (!commonStore.appLoaded)
    return <LoadingComponent content="Loading app..." />;
  return (
    <>
      <ToastContainer position="top-right" />
      <ModalContainer />
      <Route exact path="/" component={HomePage} />
      {/* <Route exact path='/fieldData' component={DataDashboard} /> */}
      <Route
        path={"/(.+)"}
        render={() => (
          <>
            <NavBar />
            <Container style={{width:"98%", paddingTop:"7em" }}>
              <Switch>
                <Route exact key={location.key} path="/administration" component={AppUserDashboard} />
                {/* <Route key={location.key} path={['/administration/create', '/administration/:id']} component={AppUserForm} /> */}
                <Route exact path="/fieldData" component={DataDashboard} />
                <Route path="/errors" component={TestErrors} />
                <Route path="/server-error" component={ServerError} />
                <Route path="/login" component={LoginForm} />
                
                
                <Route component={NotFound} />
              </Switch>
            </Container>
          </>
        )}
      />
    </>
  );
}

export default observer(App);
