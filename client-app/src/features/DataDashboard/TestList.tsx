import React from 'react';
import { Header, Menu } from 'semantic-ui-react';
import Calendar from 'react-calendar';

export default function TestList() {
    
    return (
        <>
            <Menu vertical size='large' style={{ width: '100%' }}>
                <Header icon='filter' attached color='teal' content='Tests' />
                <Menu.Item content='Field Concrete' />
                <Menu.Item content="Field Density" />
                <Menu.Item content="Field Inspection" />
            </Menu>
            
        </>
    )
}