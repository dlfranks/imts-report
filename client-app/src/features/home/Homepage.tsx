import { observer } from 'mobx-react-lite';
import React from 'react';
import { Link } from 'react-router-dom';
import { Button, Image, Container, Header, Segment } from 'semantic-ui-react';
import { useStore } from '../../app/stores/store';
import LoginForm from '../users/LoginForm';
import RegisterForm from '../users/RegisterForm';




export default observer(function HomePage() {
    const { userStore, modalStore } = useStore();
    return (
        <Segment inverted textAlign='center' vertical className='masthead'>
            <Container style={{ marginTop: '7em' }}>
                <Header as='h1' inverted>
                    <Image size='massive' src='/assets/logo.jpg' alt='logo' style={{ marginBottom: 12 }} />
                    IMTS Data Service
                </Header>
                {
                    userStore.isLoggedIn ? (
                        <>
                            <Header as='h2' inverted content='Welcome to IMTS Data Service' />
                            <Button as={Link} to='/administration' size='huge' inverted>
                                Go to Service!
                            </Button>
                        </>
                    ) : (
                            <>
                                <Button onClick={() => modalStore.openModal(<LoginForm />)} size='huge' inverted >
                                        Login!
                                </Button>
                                <Button onClick={() => modalStore.openModal(<RegisterForm />)} size='huge' inverted >
                                    Register!
                                </Button>
                            </>
                            
                    )
                }
                
            </Container>
        </Segment>
        
    )
})