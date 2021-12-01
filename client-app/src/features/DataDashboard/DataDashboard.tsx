import { Button, Grid, Item, List, Segment } from "semantic-ui-react";
import DataTable from "./DataTable";
import DataRequestForm from "./DataRequestForm";
import DataListItem from "./DataListItem";
import TestList from "./TestList";
import DataList from "./DataList";



export default function DataDashboard() {
    
   

    return (
        <Grid>
            
            <Grid.Column width='4'>
                <TestList />
            </Grid.Column>
            <Grid.Column width='12' >
                <DataList/>

            </Grid.Column>
            
        </Grid>
    )
}