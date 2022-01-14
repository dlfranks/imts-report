import React, { useEffect } from "react";
import { Container, Divider, Header } from "semantic-ui-react";

import AutoCompleteTest from "./AutoCompleteTest";
import DataListItem from "./DataListItem";
import DataRequestForm from "./DataRequestForm";
import DataTable from "./DataTable";
import data from "../../app/common/autoComplete/data.json";
import { useStore } from "../../app/stores/store";


export default function DataList() {
    
    
    
    
    return (
        
        <Container style={{ backgroundColor: '#fff', padding: '1em' }}>
            <Header as='h3' content='Concrete Data' />
            <Divider/>
            <Header as='h4' content='Full Dataset' />
            <DataTable />
            <Header as='h4' content='Mix Number Dataset' />
            <DataTable />
            <Header as='h4' content='Strength Dataset' />
            <DataTable />
            <DataRequestForm />
            <AutoCompleteTest />
            
            
        </Container>
        
            
        
    );
}