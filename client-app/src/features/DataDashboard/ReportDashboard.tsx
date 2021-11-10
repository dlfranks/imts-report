import { Grid, List } from "semantic-ui-react";
import ReportListItem from "./ReportListItem";



export default function DataDashboard() {
    
    
    return (
        <Grid>
            <Grid.Column width='16' >
                
                <List>
                    <List.Item><ReportListItem /></List.Item>
                    <List.Item><ReportListItem /></List.Item>
                    <List.Item><ReportListItem /></List.Item>
                </List>

            </Grid.Column>
            
        </Grid>
    )
}