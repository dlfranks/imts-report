import { Button, Item, Segment, Modal } from 'semantic-ui-react';
import { useStore } from '../../app/stores/store';




export default function DataListItem() {

    const { modalStore } = useStore();
    
    return (
        
        <>
            <Segment>
                <Item.Group>
                    <Item>
                        <Item.Content>
                            <Item.Header>Field Concrete Data</Item.Header>
                            <Item.Description>Retrieve concrete data from database with various format</Item.Description>
                        </Item.Content>
                        
                    </Item>
                </Item.Group>
            </Segment>
            
            <Segment>
                
            </Segment>
            
        </>
    )
}