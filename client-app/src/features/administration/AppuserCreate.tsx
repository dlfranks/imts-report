import { Formik } from 'formik';
import { observer } from 'mobx-react-lite';
import React from 'react';
import { Header, Segment } from 'semantic-ui-react';
import UserStore from '../../app/stores/userStore';


export default observer(function AppUserCreateForm() {

    const {  } = UserStore;

    return (
        <Segment clearing>
            <Header content='Application User' sub color='teal' />
            <Formik
                validationSchema={validationSchema}
                enableReinitialize
                initialValues={activity}
                onSubmit={values => handleFormSubmit(values)}>
                {({handleSubmit, isValid, isSubmitting, dirty}) => (
                    <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                        <MyTextInput placeholder='Title' name='title' />
                        <MyTextArea rows={3} placeholder='Description' name='description' />
                        <MySelectInput options={ CategoryOptions} placeholder='Category' name='category'/>
                        <MyDateInput
                            placeholderText='Date'
                            name='date'
                            showTimeSelect
                            timeCaption='time'
                            dateFormat='MMMM d, yyyy h:mm aa'
                        />
                        <MyTextInput placeholder='City' name='city' />
                        <MyTextInput placeholder='Venue' name='venue' />
                        <Button
                            disabled={isSubmitting || !dirty || !isValid}
                            loading={isSubmitting} floated='right' positive type='submit' content='Submit' />
                        <Button as={ Link} to='/activities' floated='right' type='button' content='Cancel' />
                    </Form>
                )}

            </Formik>
            
        </Segment>
    )
});