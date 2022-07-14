import React, { useEffect, useState } from "react";

import { AppUserColumnsList, IAppUser } from '../../app/models/user';
import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store";
import LoadingComponent from "../../app/layout/LoadingComponents";
import { ThemeProvider, createTheme, Paper, Chip } from "@mui/material";
import { Button } from "semantic-ui-react";
import { Link, NavLink, useHistory } from 'react-router-dom';
import { Container } from "@material-ui/core";
import MaterialTable, { MTableToolbar } from "@material-table/core";


export default observer(function AppUserDashboard() {
  const { userStore } = useStore();
  const { loadAppUsers, users, loadingInitial } = userStore;
  const defaultMaterialTheme = createTheme();
  const history = useHistory();

  

  useEffect(() => {
    loadAppUsers();
      
  }, [loadAppUsers]);

  if (loadingInitial) return <LoadingComponent content="loading" />;
  return (
    <div>
      <ThemeProvider theme={defaultMaterialTheme}>
      <MaterialTable
          components={{
            Toolbar: (props) => (
              <div>
                <MTableToolbar {...props} />
                <div style={{ padding: '0px 10px' }}>
                  <Button as={Link} primary to='/administration/create' content='Add User' style={{ marginRight: 5 }} />
                  
                </div>
              </div>
            ),
            Container: (props) => <Paper {...props} elevation={8} />,
          }}
          actions={[
            {
              icon: () => <span style={{ fontSize: 14, textDecorationLine: 'underline' }}>View</span>,
              tooltip: "View",
              onClick: async (event, rowData) => {
                if( !Array.isArray(rowData)){
                  console.log(rowData.id);
                  history.push(`/administration/${rowData.id}`);
               }
               
                
              },
            },
            
          ]}
          title="Application Users"
          columns={AppUserColumnsList}
          data={users}
        />
      </ThemeProvider>
        
      
      
    </div>
  );
});

export type Person = {
  name: string;
  surname: string;
  birthYear: number;
};

