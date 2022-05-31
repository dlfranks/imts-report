import React, { useEffect } from 'react';
import MaterialTable from "material-table";
import { AppUserColumnsList} from '../../app/models/user';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../app/stores/store';
import LoadingComponent from '../../app/layout/LoadingComponents';

import { ThemeProvider, createTheme } from '@mui/material';



export default observer(function AppUserDashboard() {

    const { userStore } = useStore();
    const { loadAppUsers, userRegistry, users, loadingInitial } = userStore;
    const defaultMaterialTheme = createTheme();


    useEffect(() => {

        if (users.length === 0) loadAppUsers();
    }, [loadAppUsers, users.length]);

    if(loadingInitial || users.length === 0) return <LoadingComponent content='loading' />
    return (
        
        <div>
            <link
                    rel="stylesheet"
                    href="https://fonts.googleapis.com/icon?family=Material+Icons"
                    />
                
            <h1>Hello CodeSandbox</h1>
            <h2>Start editing to see some magic happen!</h2>
            <ThemeProvider theme={defaultMaterialTheme}>
            <MaterialTable
                title="Application Users"
                columns={AppUserColumnsList}
                data={users}
                
                />
                </ThemeProvider>
            
        </div>

    );
})