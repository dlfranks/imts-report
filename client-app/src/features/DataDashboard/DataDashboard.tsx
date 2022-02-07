import { Container, Grid } from "semantic-ui-react";

import DataList from "./DataList";
import TestList from "./TestList";
import styled from 'styled-components';

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
