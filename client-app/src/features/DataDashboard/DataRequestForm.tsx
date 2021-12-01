
import { Formik } from "formik";
import React, { useState, useEffect } from "react";
import { Button, Form, Grid, Header, Label, Popup, Segment, Select} from "semantic-ui-react";
import * as Yup from 'yup';
import { DatasetOptions } from '../../app/common/options/formOptions';


import { Project } from '../../app/models/coreInterface';
import { useStore } from "../../app/stores/store";
import MySelectInput from '../../app/common/form/MySelectInput';
import { Autocomplete } from "@material-ui/lab";
import { TextField } from "@material-ui/core";


interface ConcreteDataParam {
    projectId: number | undefined;
    dataset: number;
}


  
export default function DataRequestForm()
{
    
    //const param: ConcreteDataParam = {projectId: 0, format: 0}
    const validationSchema = Yup.object({
        projectId: Yup.string().required('the project id is required'),
        dataset: Yup.string().required('the format is required'),
        
    });

    const { projectStore } = useStore();
    const { projects, loadProjects, loadingInitial } = projectStore;
    //const { officeId } = useParams<{ officeId: string }>();
    const [formParams, setFormParams] = useState<ConcreteDataParam>({projectId: 0, dataset: 1});
    //const [options, setOptions] = useState<Project[] | undefined>(Project[]);
    const [selectedProject, setSelectedProject] = useState<Project | null>(null);
  
    const [term, setTerm] = useState<string>('');
    
    
    
    function handleFormSubmit(formParams: ConcreteDataParam) {
        if (selectedProject)
            setFormParams({ ...formParams });
        
    }
    function requestUrl(): string {
        return `http://localhost:5000/api/FieldConcreteTest/datum?projectId=${formParams.projectId}&dataset=${formParams.dataset}`;
    }
    function excelDownload() {
        return `http://localhost:5000/api/FieldConcreteTest/excel?projectId=${formParams.projectId}&dataset=${formParams.dataset}`;
    }
    useEffect(() => {
        loadProjects().then(() => {

        })
    }, [loadProjects]);

    return (
        <Segment clearing>
                <Formik 
                    initialValues={formParams}
                    onSubmit={values => {handleFormSubmit(values)}}
            >
                {
                    
                    ({ handleSubmit, isSubmitting, values, setFieldValue, setValues }) => (
                        
                        <form onSubmit={handleSubmit} autoComplete='off'>
                            
                            <Grid columns={2}>
                                <Grid.Row>
                                    <Grid.Column>
                                        
                                            <label>Project</label>
                                            <Autocomplete
                                                disablePortal
                                                id="combo-box-demo"
                                                style={{margin:0, padding:0}}
                                                options={projects}
                                                getOptionLabel={option => option.name}
                                                value={selectedProject}
                                                
                                                // getOptionSelected={(option, value) => {
                                                    
                                                //     return option.id === value.id
                                                    
                                                // }}
                                                // renderOption={(option: Project) => (
                                                // <Box display="flex" flexDirection="row" alignItems="center">
                                                    
                                                //     <Box>
                                                //     <Typography variant="body2">{option.name}</Typography>
                                                //     </Box>
                                                // </Box>
                                                // )}
                                                onChange={(e: object, value: any | null) => {
                                                    if (value) {
                                                        console.log('do the types match?', typeof value.id === typeof values.projectId);
                                                    
                                                        setSelectedProject(value);
                                                        setFieldValue("projectId", value.id);
                                                    }
                                                    
                                                }}
                                                renderInput={params => (
                                                    <TextField margin='dense'
                                                    
                                                    {...params}
                                                        name="name"
                                                        variant='outlined'
                                                    
                                                    
                                                    
                                                />
                                               )}
                                            />
                                            <pre>{ JSON.stringify(values, null, 2)}</pre>
                                        
                                    </Grid.Column>
                                    <Grid.Column>
                                                
                                            <MySelectInput
                                                name='dataset'
                                                label='Dataset'
                                                options={DatasetOptions}
                                                placeholder='Dataset'
                                            />
                                        
                                        
                                    </Grid.Column>
                                </Grid.Row>
                                <Grid.Row>
                                    <Grid.Column width={16}>
                                        <Popup 
                                            content={
                                                

                                                
                                                <div className='api_popup'>
                                                    <Header as='h4'>Access this Dataset via API</Header>
                                                    
                                                    <div style={{clear:'both', marginBottom:'1em'}}>
                                                        CSV: 
                                                        <div className="ui input" style={{ width: "400px", marginLeft:'1em' }}><input type="text"
                                                            value={`http://localhost:5000/api/FieldConcreteTest/datum?projectId=${formParams.projectId}&dataset=${formParams.dataset}`}
                                                        /></div>
                                                        <button className="ui button" onClick={() =>
                                                            navigator.clipboard.writeText(requestUrl())}
                                                        >Copy</button>
                                                    </div>
                                                    <div style={{clear:'both', marginBottom:'1em'}}>
                                                        API Endpoint: 
                                                        <div className="ui input" style={{ width: "400px", marginLeft:'1em' }}><input type="text"
                                                            value={`http://localhost:5000/api/FieldConcreteTest/datum?projectId=${formParams.projectId}&dataset=${formParams.dataset}`}
                                                        /></div>
                                                        <button className="ui button" onClick={() =>
                                                            navigator.clipboard.writeText(requestUrl())}
                                                        >Copy</button>
                                                    </div>
                                                    
                                            </div>
                                            }
                                            on='click'
                                            positionFixed
                                            trigger={<Button>API</Button>}
                                        />
                                        <Button color='teal'>Json</Button>
                                        <Button color='teal' content='Excel' />
                                        <Button color='teal' content='XML'/>
                                    </Grid.Column>
                                </Grid.Row>
                                
                            </Grid>
                                
                                
                                
                               
                           
                            
                            
                            
                            
                        </form>
                    )
                }

                </Formik>
        </Segment>
    )
}