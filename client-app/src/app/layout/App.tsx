import React, { useEffect } from 'react';
import { Container} from 'semantic-ui-react';
import DataDashboard from '../../features/DataDashboard/DataDashboard';
import ModalContainer from '../common/modals/ModalContainer';
import NavBar from './NavBar';
import HomePage from '../../features/home/Homepage';
import { Route, Switch, useLocation } from 'react-router-dom';
import { useStore } from '../stores/store';
import TestErrors from '../../features/errors/TestErrors';
import ServerError from '../../features/errors/ServerError';
import LoginForm from '../../features/users/LoginForm';
import NotFound from '../../features/errors/NotFound';
import AppUserDashboard from '../../features/administration/AppUserDashboard';


function App() {
  const location = useLocation();
  const { commonStore, userStore } = useStore();

  useEffect(() => {
    if (commonStore.token) {
      userStore.getUser().finally(() => commonStore.setAppLoaded());
    } else {
      commonStore.setAppLoaded();
    }
  }, [commonStore, userStore]);

  //if(!commonStore.appLoaded) return <LoadingComponent content='Loading app...' />
  return (
    <>
      <ModalContainer/>
      <Route exact path='/' component={HomePage} />
      {/* <Route exact path='/fieldData' component={DataDashboard} /> */}
      <Route
        path={'/(.+)'}
        render={() => (
          <>
            <NavBar />
            <Container style={{ marginTop: '7em' }}>
              <Switch>
                <Route exact path='/fieldData' component={DataDashboard} />
                <Route path='/errors' component={TestErrors} />
                <Route path='/server-error' component={ServerError} />
                <Route path='/login' component={LoginForm} />
                <Route path='/administration' component={AppUserDashboard}/>
                <Route component={NotFound} />
              </Switch>
            </Container>
            
                
          </>
        )}
      />
    </>
  );
}

export default App;
