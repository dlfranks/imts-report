import React, { useEffect } from "react";
import { Container, Divider, Header } from "semantic-ui-react";

import AutoCompleteTest from "./AutoCompleteTest";

import DataRequestForm from "./DataRequestForm";


import { useStore } from "../../app/stores/store";
import MyTable from '../../app/common/table/MyTable';
import { observer } from "mobx-react-lite";






export default observer(function DataList() {
    
    const { concreteStore } = useStore();
    const { samples, getSamples, sampleReady } = concreteStore;

    const headers  = (data: {}) => {
        

        let list:{[key:string]: string}= {};
        Object.keys(data).map((key) =>  {
            list[key.toString()] = key;
            
                
                
        });
        
        console.log(list);

        return list ;
        

    }
    
    useEffect(() => {
        //if (id) loadActivity(id).then(activity => setActivity(activity!))
        if (!sampleReady) {
            getSamples();
        }

    }, [samples, getSamples, sampleReady]);

    if (!sampleReady) return null;
    
    return (
        <Container style={{ backgroundColor: '#fff', padding: '1em 2em' }}>
            <Container >
                <Header as='h3' content='Concrete Data' />
                <Divider />
                <Header as='h4' content='Full Dataset' />
                    <MyTable items={ samples.full} headers={headers(samples.full[0])}/>
                    <Header as='h4' content='Mix Number Dataset' />
                    <MyTable items={samples.strength} headers={headers(samples.strength[0])}/>
                    <Header as='h4' content='Strength Dataset' />
                    <MyTable items={samples.mixNumber}  headers={headers(samples.mixNumber[0])}/>
            </Container>
            <Container style={{margin:'5em 0'}}>
                <DataRequestForm />
                
            </Container>
        </Container>
            
        
    );
})