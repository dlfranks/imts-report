import { Container, Grid } from "semantic-ui-react";


import styled from 'styled-components';
import DataList from "./DataList";
import TestList from "./TestList";

export default function DataDashboard() {
    
    return (
        
            <Grid>
            
            <Grid.Column width='4'>
                <TestList />
            </Grid.Column>
            <Grid.Column width='12' >
                <DataList />
            </Grid.Column>
            
            </Grid>
        
        
    )
}
