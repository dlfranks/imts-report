import {Grid } from "semantic-ui-react";
import DataList from "./DataList";
import TestSideMenu from './TestSideMenu';

export default function DataDashboard() {
    
    return (
        
            <Grid>
            <Grid.Column width='4'>
                <TestSideMenu />
            </Grid.Column>
            <Grid.Column width='12' >
                <DataList />
            </Grid.Column>
            </Grid>
        
        
    )
}
