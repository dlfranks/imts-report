import { Button, Grid, Item, List, Segment } from "semantic-ui-react";
import DataTable from "./DataTable";
import DataRequestForm from "./DataRequestForm";
import DataListItem from "./DataListItem";
import TestList from "./TestList";
import DataList from "./DataList";
import { useStore } from "../../app/stores/store";
import { useEffect } from "react";



export default function DataDashboard() {
    const { concreteStore } = useStore();
    

    

    
   

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
