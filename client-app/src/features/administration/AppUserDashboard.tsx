import React, { useEffect } from "react";
import MaterialTable, { MTableToolbar } from "material-table";
import { AppUserColumnsList } from "../../app/models/user";
import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store";
import LoadingComponent from "../../app/layout/LoadingComponents";
import { ThemeProvider, createTheme, Paper, Button, Chip } from "@mui/material";

export default observer(function AppUserDashboard() {
  const { userStore } = useStore();
  const { loadAppUsers, users, loadingInitial } = userStore;
  const defaultMaterialTheme = createTheme();

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
                <div style={{padding: '0px 10px'}}>
                  <Chip label="Add User" color="secondary" style={{marginRight: 5}}/>
                  
                </div>
              </div>
            ),
            Container: (props) => <Paper {...props} elevation={8} />,
          }}
          actions={[
            {
              icon: () => <span style={{ fontSize: 14, textDecorationLine:'underline' }}>View</span>,
              tooltip: "View",
              onClick: (event, rowData) => {
                // Do save operation
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
