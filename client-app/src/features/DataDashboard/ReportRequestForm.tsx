import { Formik } from "formik";
import React from "react";
import { Button, Form, Header, Segment } from "semantic-ui-react";
import * as Yup from 'yup';

interface Props {
    projectId: number;
    format: number;
}

export default function ReportRequestForm()
{
    const param: Props = {projectId: 0, format: 0}
    const validationSchema = Yup.object({
        projectId: Yup.string().required('the project id is required'),
        format: Yup.string().required('the format is required'),
        
    });
    
    function handleFormSubmit() {
        
    }

    return (
        <Segment>
                <Formik validationSchema={validationSchema}
                    initialValues={param}
                    onSubmit={values => {handleFormSubmit()}}
            >
                {
                    ({ handleSubmit }) => (
                        <Form class='ui form' onSubmit={handleSubmit} autoComplet='off'>
                            <Header as='h2' content='Export Concrete Data' color='teal' textAlign='center' />
                            <Form.Field>
                                <label>Project</label>
                                <input placeholder='Project name' type='text' />
                            </Form.Field>
                            <Button floated='right' type='submit' content='Submit' />
                            <Button floated='right' type='button' content='Cancel' />
                            
                        </Form>
                    )
                }

                </Formik>
        </Segment>
    )
}