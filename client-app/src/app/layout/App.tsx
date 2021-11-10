import React from 'react';
import { Header, List } from 'semantic-ui-react';
import DataDashboard from '../../features/DataDashboard/ReportDashboard';
import ModalContainer from '../common/modals/ModalContainer';


function App() {
  return (
    <>
      <ModalContainer/>
      <div className="App">
        <Header as='h2' icon='users' content='Imts Data Portal' />
        <DataDashboard/>
        
        </div>
      </>
  );
}

export default App;
