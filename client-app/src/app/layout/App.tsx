import React from 'react';
import { Container, Header, List } from 'semantic-ui-react';
import DataDashboard from '../../features/DataDashboard/DataDashboard';
import TestList from '../../features/DataDashboard/TestList';
import ModalContainer from '../common/modals/ModalContainer';
import NavBar from './NavBar';


function App() {
  return (
    <>
      <ModalContainer/>
      <div className="App">
        <NavBar />
        <Container style={{marginTop: '7em'}}>
          <DataDashboard />
          
        </Container>
        
        
        </div>
      </>
  );
}

export default App;
