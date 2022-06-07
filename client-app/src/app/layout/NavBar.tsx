//import { Link, NavLink } from 'react-router-dom';
import { observer } from "mobx-react-lite";
import { NavLink } from "react-router-dom";
import { Container, Menu, Button, Image, Dropdown } from "semantic-ui-react";
import { useStore } from "../stores/store";
import UserStore from "../stores/userStore";
import { IDValuePair, Office } from "../models/coreInterface";
import MySelectInput from "../common/form/MySelectInput";
import { isError } from "lodash";
import { useState } from "react";

export default observer(function NavBar() {
  const {
    userStore: { user, logout, switchOffice },
    } = useStore();

    const officeOptions =
        user?.memberOffices.map((office) => {
            if (office.id === user.officeId) {
                return {
                    key: office.id,
                    text: office.name,
                    value: office.id,
                    selected: true,
                    active: true
                };
            } else {
                return {
                    key: office.id,
                    text: office.name,
                    value: office.id
                };
            }
            
        });
    

    
  return (
    <Menu inverted fixed="top">
      <Container>
        <Menu.Item header exact as={NavLink} to="/">
          <img
            src="/assets/logo.jpg"
            alt="logo"
            style={{ marginRight: "10px" }}
          />
          Imts Data Portal
        </Menu.Item>
        <Menu.Item as={NavLink} to="/fieldData" name="FieldDataService" />
        <Menu.Item as={NavLink} to="/errors" name="Errors" />
        <Menu.Item as={NavLink} to="/administration" name="administration" />
        <Menu.Item>
          <Dropdown
            pointing="top right"
            options={officeOptions}
            selection
            text={user?.memberOffices[user.officeId - 1].name}
            onChange={(e, d) => {
              const officeId: any = d.value;
              switchOffice(officeId);
            }}
          />
        </Menu.Item>
        <Menu.Item position="right">
          <Image
            src={user?.image || "/assets/user.png"}
            avatar
            spaced="right"
          />
          <Dropdown pointing="top left" text={`${user?.displayName}`}>
            <Dropdown.Menu>
              <Dropdown.Item
                to={`/profile/${user?.username}`}
                text="My Profile"
                icon="user"
              />
              <Dropdown.Item onClick={logout} text="Logout" icon="power" />
            </Dropdown.Menu>
          </Dropdown>
        </Menu.Item>
      </Container>
    </Menu>
  );
});
