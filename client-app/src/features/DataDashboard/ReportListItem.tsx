import { Button, Item, Segment, Modal } from 'semantic-ui-react';
import { useStore } from '../../app/stores/store';
import ReportRequestForm from './ReportRequestForm';



export default function DataListItem() {

    const { modalStore } = useStore();
    
    return (
        <Segment.Group>
            <Segment>
                <Item.Group>
                    <Item>
                        <Item.Content>
                            <Item.Header>Field Concrete Data</Item.Header>
                            <Item.Description>Retrive concrete data with various format</Item.Description>
                        </Item.Content>
                        
                    </Item>
                </Item.Group>
            </Segment>
            
            <Segment>
                <Button onClick={() => modalStore.openModal(<ReportRequestForm/>)} color='teal' inverted>Json</Button>
                <Button color='teal' content='Excel' />
                <Button color='teal' content='XML'/>
            </Segment>
            
        </Segment.Group>
    )
}