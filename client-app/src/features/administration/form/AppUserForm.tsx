import { Formik, FormikProps } from "formik";
import { observer } from "mobx-react-lite";
import React, { useState, useEffect, SyntheticEvent } from "react";
import { Button, Form, Header, Segment } from "semantic-ui-react";
import MySelectInput from "../../../app/common/form/MySelectInput";
import MyTextInput from "../../../app/common/form/MyTextInput";
import { useStore } from "../../../app/stores/store";
import { AppUser} from "../../../app/models/user";
import * as Yup from "yup";
import { RoleOptions } from "../../../app/common/options/Urls";
import { Link, useParams, useHistory } from 'react-router-dom';
import MyCheckbox from "../../../app/common/form/MyCheckbox";
import { toast } from "react-toastify";


export default observer(function AppUserForm() {
  const history = useHistory();
  const { userStore, commonStore } = useStore();
  const { createAppUser, updateAppUser, loadAppUser, lookupUsername,  deleteAppUser} = userStore;
  const { id } = useParams<{ id: string }>();
  const [appUser, setAppUser] = useState<AppUser>(new AppUser());
  const [createMode, setCreateMode] = useState<boolean>(false);

  const validationSchema = Yup.object({
    firstName: Yup.string().required("Required"),
    lastName: Yup.string().required("Required"),
    email: Yup.string().required("Required").email(),
    roleName: Yup.string().required("Required"),
  });

  function handleFormSubmit(appUser: AppUser) {

    if (!id){
      let newAppuser = {
        ...appUser,
      };
      createAppUser(newAppuser).then(() => {
        history.push(`/administration`);
      });
    } else {

      updateAppUser(appUser).then(() => {
        history.push(`/administration`);
      })
      }
    
  }

  const showAndHideElement = (props: FormikProps<any>) => {
    if (props.values.isImtsUser)
      return (
        <MyTextInput
          placeholder="Imts Username"
          name="imtsUserName"
          label="IMTS Username"
        />
      );
    else return null;
  };

  const handleLookupUserName = (
    email: string,
    e: SyntheticEvent<HTMLButtonElement>
  ) => {
    e.preventDefault();
    lookupUsername(email).then((result) => {
      if (result?.isValidToCreate) {
        if (result.appUserDTO)
          setAppUser(result.appUserDTO);
        else {
          let user = new AppUser();
          setAppUser({...user, email: email});
        }
        setCreateMode(true);
        toast.success(result.succmsg, {autoClose: false});
      } else {
        setCreateMode(false);
        toast.error(result?.errmsg, {autoClose: false});
      }
    });
  };
  const changeHandlerIsImtsUser = (props: FormikProps<any>) => {
    let user = { ...props.values, isImtsUser: !props.values.isImtsUser};
    setAppUser(user);
    
  };
  const changeHandlerPassword = () => {
    let newUserOrSelf = false;
    const currentuser = commonStore.user;
    const targetUser = appUser;
    if (currentuser?.id === targetUser.id) newUserOrSelf = true;
    else if (!appUser.id) newUserOrSelf = true;
    return newUserOrSelf ? (
      <MyTextInput
        label="Password"
        type="password"
        name="password"
        placeholder="Password"
      />
    ) : null;
  };

  const formElements = (props: FormikProps<any>) => {
    if (appUser.id || createMode)
      return (
        <>
          <MyTextInput
            placeholder="First Name"
            name="firstName"
            label="First Name"
          />
          <MyTextInput
            placeholder="Last Name"
            name="lastName"
            label="Last Name"
          />

          <MySelectInput
            options={RoleOptions}
            placeholder="Select a Role"
            name="roleName"
            label="Role"
          />
          {changeHandlerPassword()}

          <MyCheckbox
            name="isImtsUser"
            label="Are you a IMTS user?"
            type="checkbox"
            onChange={() => changeHandlerIsImtsUser(props)}
          />

          {showAndHideElement(props)}

          <Button
            disabled={props.isSubmitting || !props.isValid}
            loading={props.isSubmitting}
            floated="right"
            positive
            type="submit"
            content="Submit"
          />
          <Button
            as={Link}
            to="/administration"
            floated="right"
            type="button"
            content="Cancel"
          />
          <Button
            negative
            floated="right"
            type="button"
            content="Delete"
            onClick={() => {
              if (id)
              {
                deleteAppUser(id).then(() => {
                  history.push('/administration');
                });
              }
              
            }}
          />
        </>
      );
    else return null;
  };

  const lookupButtonVisibility = () => (createMode === false) && !(appUser.id) ? 'display' : 'none';
  const lookupEmailVisibility = () => (createMode === false) && !(appUser.id) ? false : true;

  useEffect(() => {
    if (id) {
      loadAppUser(id).then((appUser) => {
        setAppUser(new AppUser(appUser));
      });
    }
  }, [id, loadAppUser]);

  return (
    <Segment clearing>
      <Header
        as="h1"
        content="Application User"
        sub
        color="teal"
        style={{ marginBottom: "3em" }}
      />
      <Formik
        validationSchema={validationSchema}
        enableReinitialize
        initialValues={appUser}
        onSubmit={(values) => handleFormSubmit(values)}
      >
        {(props: FormikProps<any>) => (
          <Form
            className="ui form"
            onSubmit={props.handleSubmit}
            autoComplete="off"
          >
            <Segment>
              <MyTextInput
                placeholder="Emial"
                name="email" label="Email"
                disabled={lookupEmailVisibility()}
              />
              <Button
                primary
                onClick={(e) => handleLookupUserName(props.values.email, e)}
                //disabled={props.values.email?.length === 0 || props.values.id}
                style={{ display: lookupButtonVisibility() }}
              >
                Lookup User
              </Button>
            </Segment>
            {formElements(props)}
          </Form>
        )}
      </Formik>
    </Segment>
  );
});
