import React, { useEffect } from "react";
import { Container, Divider, Header } from "semantic-ui-react";

import AutoCompleteTest from "./AutoCompleteTest";
import DataListItem from "./DataListItem";
import DataRequestForm from "./DataRequestForm";
import DataTable from "./DataTable";
import data from "../../app/common/autoComplete/data.json";
import { useStore } from "../../app/stores/store";
import { LooseObject } from '../../app/stores/concreteStore';
import { ConcreteTableSamples } from "../../app/models/concreteInterface";


export default function DataList() {
    
    const { concreteStore } = useStore();
    const { samples, getSamples, sampleReady } = concreteStore;
    
    useEffect(() => {
        //if (id) loadActivity(id).then(activity => setActivity(activity!))
        if (!sampleReady) {
            getSamples();
        }

    }, [samples, getSamples, sampleReady]);

    //if (!sampleReady) return null;
    
    return (
        
        <Container style={{ backgroundColor: '#fff', padding: '1em' }}>
            <Header as='h3' content='Concrete Data' />
            <Divider />
            <Header as='h4' content='Full Dataset' />
                <DataTable data={ samples.full}/>
                <Header as='h4' content='Mix Number Dataset' />
                <DataTable data={samples.strength}/>
                <Header as='h4' content='Strength Dataset' />
                <DataTable data={samples.mixNumber} />
            
            <DataRequestForm />
            <AutoCompleteTest />
            
            
        </Container>
        
            
        
    );
}