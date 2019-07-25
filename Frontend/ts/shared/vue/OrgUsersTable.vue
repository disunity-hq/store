<template>
  <div>
    <table class="table">
      <thead>
        <tr>
          <th scope="col">Username</th>
          <th scope="col">Role</th>
          <th scope="col" class="smallCell" v-if="canManageRoles">
            <button type="button" class="btn btn-light" v-on:click="addingMember = !addingMember">
              <i class="fas fa-plus" />
            </button>
          </th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="membership in members" v-bind:key="membership.username">
          <td>{{ membership.userName }}</td>
          <td>{{membership.role}}</td>
          <td class="smallCell" v-if="canManageRoles">
            <button
              type="button"
              class="btn btn-light"
              v-if="membership.role !== 'Owner'"
              v-on:click="removeMember(membership.userName)"
            >
              <i class="fas fa-minus" />
            </button>
          </td>
        </tr>
        <tr v-if="addingMember">
          <td>
            <div class="form-group">
              <input
                type="text"
                class="form-control"
                placeholder="Enter Username..."
                v-model="userName"
              />
            </div>
          </td>
          <td>
            <div class="form-group">
              <select class="form-control" v-model="role">
                <option v-for="role in roles" v-bind:key="role">{{role}}</option>
              </select>
            </div>
          </td>
          <td class="smallCell">
            <button type="button" class="btn btn-light" v-on:click="addMember">
              <i class="fas fa-plus" />
            </button>
            <button type="button" class="btn btn-light" v-on:click="addingMember = false">
              <i class="fas fa-minus" />
            </button>
          </td>
        </tr>
      </tbody>
    </table>
    <div id="orgMembersErrors"></div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { Component, Prop } from "vue-property-decorator";
import axios from "axios";
import { Button } from "@syncfusion/ej2-buttons";
import ErrorReporter from "../ErrorReporter";

enum MemberRole {
  Member,
  Admin,
  Owner
}

interface IMembership {
  userName: string;
  role: string;
}

@Component
export default class OrgMembersTable extends Vue {
  members: IMembership[] = [];
  addingMember = false;

  roles = Object.keys(MemberRole).filter(k => isNaN(Number(k)));

  @Prop({ type: Boolean, default: false }) readonly canManageRoles: boolean;
  @Prop({ type: Boolean, default: false }) readonly canManageMembers: boolean;
  @Prop({ type: String, required: true }) readonly orgSlug: string;

  userName: string = "";
  role: string = "Member";

  readonly baseUrl = `/api/v1/orgs/${this.orgSlug}/members`;

  readonly errorReporter = new ErrorReporter("#orgMembersErrors");

  public async created() {
    const addButton = new Button(
      {
        iconCss: "fas fa-plus"
      },
      "#addUserButton"
    );

    try {
      const members = (await axios.get<IMembership[]>(this.baseUrl)).data;
      if (Array.isArray(members)) {
        this.members = members;
      }
    } catch (e) {
      console.error(e);
    }
  }

  public async addMember() {
    const membership: IMembership = {
      userName: this.userName,
      role: this.role
    };
    try {
      const response = await axios.post(this.baseUrl, membership);
      if (response.status === 204) {
        this.members.push(membership);
        this.addingMember = false;
        this.userName = "";
        this.role = "Member";
      } else {
        this.errorReporter.Empty();
        this.errorReporter.Set(response.data);
      }
    } catch (e) {
      console.error(e);
    }
  }

  public async removeMember(username: string) {
    try {
      const response = await axios.delete(`${this.baseUrl}/${username}`);

      if (response.status === 204) {
        this.members = this.members.filter(m => m.userName != username);
      } else {
        this.errorReporter.Empty();
        this.errorReporter.Set(response.data);
      }
    } catch (e) {
      console.error(e);
    }
  }
}
</script>
