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
    const { samples, getSamples, sampleReady } = concreteStore;

    const dataList = () => {
        if(samples && samples.length > 0){
            return (
            <Grid.Column width='12' >
            <DataList />)
            </Grid.Column>
           )
            }
    }

    useEffect(() => {
        //if (id) loadActivity(id).then(activity => setActivity(activity!))
        if (!sampleReady) {
            getSamples().then((samples) => {
                
            });
        }

    }, [samples, getSamples, sampleReady]);

    //if (!sampleReady) return (<LoadingComponent content="Loading activities..." />);
   

    return (
        <Grid>
            
            <Grid.Column width='4'>
                <TestList />
            </Grid.Column>
            {
                
                
                dataList()
            
            }
            
            
        </Grid>
    )
}