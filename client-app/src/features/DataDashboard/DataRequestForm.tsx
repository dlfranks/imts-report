import React, { useState, useEffect, SyntheticEvent } from "react";
import {
  Button,
  Form,
  Grid,
  Header,
  Popup,
  DropdownProps,
} from "semantic-ui-react";
import * as Yup from "yup";
import { ConcreteDatasetOptions } from "../../app/models/concreteInterface";
import { Project } from "../../app/models/coreInterface";
import { useStore } from "../../app/stores/store";
import { Autocomplete } from "@material-ui/lab";
import { TextField } from "@material-ui/core";
import { ConcreteParam } from "../../app/models/concreteInterface";
import { toast } from "react-toastify";

export default function DataRequestForm() {
  //const param: ConcreteDataParam = {projectId: 0, format: 0}
  const validationSchema = Yup.object({
    projectId: Yup.string().required("the project id is required"),
    dataset: Yup.string().required("the format is required"),
  });

  const { projectStore, concreteStore } = useStore();
  const { projects, loadProjects } = projectStore;
  const [selectedProject, setSelectedProject] = useState<Project | null>(null);
  const [formParams, setFormParams] = useState<ConcreteParam>({
    projectId: 0,
    dataset: 1,
    format: "",
  });

  function validateForm() {
    if (
      formParams.projectId > 0 &&
      formParams.dataset > 0 &&
      formParams.dataset < 4
    )
      return true;
    else {
      toast.error("Project and dataset are required.", { autoClose: false });
      return false;
    }
  }
  function handleJson() {
    if (validateForm()) concreteStore.getJson(formParams);
  }
  function handleXml() {
    concreteStore.downloadXml(formParams);
  }
  function handleExcel() {
    concreteStore.downloadExcel(formParams);
  }
  const handleDataSetChange = (
    event: SyntheticEvent<HTMLElement | Event>,
    data: DropdownProps
  ) => {
    const { name, value } = data;

    setFormParams({ ...formParams, [name]: value });
  };

  useEffect(() => {
    loadProjects().then(() => {});
  }, [loadProjects]);

  return (
    <div>
      <Grid columns={2}>
        <Grid.Row>
          <Grid.Column>
            <label>Project</label>
            <Autocomplete
              disablePortal
              id="combo-box-demo"
              style={{ margin: 0, padding: 0 }}
              options={projects}
              getOptionLabel={(option) => option.name}
              value={selectedProject}
              onChange={(e: object, value: Project | null) => {
                if (value) {
                  console.log(
                    "do the types match?",
                    typeof value.id === typeof formParams.projectId
                  );

                  setSelectedProject(value);
                  setFormParams({ ...formParams, projectId: value.id });
                }
              }}
              renderInput={(params) => (
                <TextField
                  margin="dense"
                  {...params}
                  name="name"
                  variant="outlined"
                />
              )}
            />
            <pre>{JSON.stringify(formParams, null, 2)}</pre>
          </Grid.Column>
          <Grid.Column>
            <Form.Field>
              <label>Data set</label>
              <Form.Select
                style={{ marginTop: "7px", display: "block" }}
                placeholder="DataSet"
                value={formParams.dataset}
                name={"dataset"}
                onChange={handleDataSetChange}
                options={ConcreteDatasetOptions}
              />
            </Form.Field>
          </Grid.Column>
        </Grid.Row>
        <Grid.Row>
          <Grid.Column width={16}>
            <Popup
              content={
                <div className="api_popup">
                  <Header as="h4">Access this Dataset via API</Header>

                  <div style={{ clear: "both", marginBottom: "1em" }}>
                    Josn Endpoint:
                    <div
                      className="ui input"
                      style={{ width: "400px", marginLeft: "1em" }}
                    >
                      <input
                        type="text"
                        value={`http://localhost:5000/api/FieldConcreteTest/json?projectId=${formParams.projectId}&dataset=${formParams.dataset}`}
                      />
                    </div>
                    <button
                      className="ui button"
                      onClick={() =>
                        navigator.clipboard.writeText(
                          `http://localhost:5000/api/FieldConcreteTest/json?projectId=${formParams.projectId}&dataset=${formParams.dataset}`
                        )
                      }
                    >
                      Copy
                    </button>
                  </div>
                  <div style={{ clear: "both", marginBottom: "1em" }}>
                    Excel Endpoint:
                    <div
                      className="ui input"
                      style={{ width: "400px", marginLeft: "1em" }}
                    >
                      <input
                        type="text"
                        value={`http://localhost:5000/api/FieldConcreteTest/excel?projectId=${formParams.projectId}&dataset=${formParams.dataset}`}
                      />
                    </div>
                    <button
                      className="ui button"
                      onClick={() =>
                        navigator.clipboard.writeText(
                          `http://localhost:5000/api/FieldConcreteTest/excel?projectId=${formParams.projectId}&dataset=${formParams.dataset}`
                        )
                      }
                    >
                      Copy
                    </button>
                  </div>
                </div>
              }
              on="click"
              positionFixed
              trigger={<Button>API</Button>}
            />
            <Button color="teal" onClick={handleJson}>
              Json
            </Button>
            <Button color="teal" content="Excel" onClick={handleExcel} />
            <Button color="teal" content="XML" onClick={handleXml} />
          </Grid.Column>
        </Grid.Row>
      </Grid>
    </div>
  );
}
